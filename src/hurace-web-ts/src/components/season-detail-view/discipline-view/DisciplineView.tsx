import React from 'react';
import styled from 'styled-components';
import { DisciplineViewItem } from './DisciplineViewItem';
import { getRacesForSeason } from '../../../common/api';
import { useStateAsync } from '../../../hooks/asyncState';

const DisciplinePanel = styled.div`
    margin-top: 10px;
    width: 100%;
    overflow: auto;
`;

export const DisciplineView: React.FC<{ seasonId: number }> = ({
    seasonId
}) => {
    const [disciplineData] = useStateAsync(getRacesForSeason, seasonId);

    return (
        <DisciplinePanel>
            {disciplineData?.map(d => (
                <DisciplineViewItem
                    disciplineData={d}
                    seasonId={seasonId}
                    key={d.discipline.id}
                ></DisciplineViewItem>
            ))}
        </DisciplinePanel>
    );
};
