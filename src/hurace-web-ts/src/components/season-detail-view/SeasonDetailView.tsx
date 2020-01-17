import React from 'react';
import { DetailViewWrapper } from '../shared/DetailViewWrapper';
import { SeasonDetailPanel } from './SeasonDetailPanel';
import { DisciplineView } from './discipline-view/DisciplineView';
import { RowFlex, ColumnFlex } from '../../theme/CustomComponents';
import { deleteSeason } from '../../common/api';
import { useHistory } from 'react-router-dom';

export const SeasonDetailView: React.FC<{ seasonId: number }> = ({
    seasonId
}) => {
    const history = useHistory();

    return (
        <DetailViewWrapper
            url="/season"
            backText="Zurück zur Saisonübersicht"
            editConfig={{
                editText: 'Saison bearbeiten',
                editUrl: `/season/${seasonId}/update`
            }}
            deleteConfig={{
                deleteText: 'Saison löschen',
                deleteFunc: async () => {
                    await deleteSeason(seasonId);
                    history.push('/season');
                }
            }}
        >
            <ColumnFlex>
                <RowFlex>
                    <SeasonDetailPanel seasonId={seasonId} />
                </RowFlex>
                <DisciplineView seasonId={seasonId} />
            </ColumnFlex>
        </DetailViewWrapper>
    );
};
