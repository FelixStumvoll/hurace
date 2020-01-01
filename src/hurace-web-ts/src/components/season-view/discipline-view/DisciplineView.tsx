import React, { useEffect, useState } from 'react';
import styled from 'styled-components';
import { DisciplineData } from '../../../interfaces/DisciplineData';
import { DisciplineViewItem } from './DisciplineViewItem';
import { setStateAsync } from '../../../common/stateSetter';
import { getRacesForSeason } from '../../../api';

const DisciplinePanel = styled.div`
    margin-top: 20px;
    max-height: 500px;
    width: 100%;
    overflow: auto;
`;

export const DisciplineView: React.FC<{ seasonId: number }> = ({
    seasonId
}) => {
    const [disciplineData, setDisciplineData] = useState<
        DisciplineData[] | undefined
    >(undefined);

    useEffect(() => {
        if (disciplineData !== undefined) return;
        setStateAsync(setDisciplineData, getRacesForSeason(seasonId));
    }, [disciplineData, seasonId]);

    return (
        <DisciplinePanel>
            {disciplineData?.map(d => (
                <DisciplineViewItem
                    disciplineData={d}
                    key={d.discipline.id}
                ></DisciplineViewItem>
            ))}
        </DisciplinePanel>
    );
};
