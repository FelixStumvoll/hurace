import React from 'react';
import styled from 'styled-components';
import { DisciplineViewItem } from './DisciplineViewItem';
import { getRacesForSeason } from '../../../common/api';
import { useAsync } from 'react-async-hook';
import { LoadingWrapper } from '../../shared/LoadingWrapper';
import { compareDiscipline } from '../../../common/compareFunctions';

const DisciplinePanel = styled.div`
    margin-top: ${props => props.theme.gap};
    width: 100%;
    overflow: auto;
`;

export const DisciplineView: React.FC<{ seasonId: number }> = ({
    seasonId
}) => {
    const {
        loading,
        error,
        result: disciplineData
    } = useAsync(getRacesForSeason, [seasonId]);

    return (
        <DisciplinePanel>
            <LoadingWrapper loading={loading} error={error}>
                {disciplineData
                    ?.sort((d1, d2) =>
                        compareDiscipline(d1.discipline, d2.discipline)
                    )
                    .map(d => (
                        <DisciplineViewItem
                            disciplineData={d}
                            seasonId={seasonId}
                            key={d.discipline.id}
                        ></DisciplineViewItem>
                    ))}
            </LoadingWrapper>
        </DisciplinePanel>
    );
};
